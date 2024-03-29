# EFCoreConcurrentCheckTest

本文主要测试[文档](https://docs.microsoft.com/en-us/ef/core/saving/concurrency)上的并发冲突。
先确保能捕获到异常，然后结合[Polly](https://github.com/App-vNext/Polly)库，可以对关键的代码做一些重试的操作。

大致分以下几步：
1. 数据库表增加一个timestamp字段，取名RowVersion，对应DbContext里面的model就是DateTime。
2. 配置这个字段在Add或者Update的时候会自己赋值。数据库修改方式[链接](https://blog.csdn.net/w405722907/article/details/85768660)
3. 给这个RowVersion字段增加ConcurrentCheck，分两种，效果是一样的。
```csharp
[ConcurrencyCheck]
public DateTime? RowVersion { get; set; }
```
或者
```csharp
entity.Property(e => e.RowVersion)
      .HasColumnType("timestamp")
      .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
      .ValueGeneratedOnAddOrUpdate()
      .IsConcurrencyToken()
      ;
```
4. 然后在代码上SaveChange的时候，生成的语句就会多一个RowVersion=@p的条件，用乐观锁的方式来避免并发的问题。因为如果其他的client更新了，那么update受影响的行数就会是0不是1，会抛出一个`DbUpdateConcurrencyException`异常。
5. 给语句增加日志的方法：[链接](https://docs.microsoft.com/en-us/ef/core/miscellaneous/logging)
6. 连接字符串配置到环境变量里面，用如下方式获取

```csharp
//修改之后，不晓得何故，要重启电脑才能生效
string connStr = Environment.GetEnvironmentVariable("MYSQLCONNSTR");
```

7. 生成DbContext的方法

```bash
dotnet ef dbcontext scaffold "连接字符串，server=xxx;password=xxx等" "Pomelo.EntityFrameworkCore.MySql" -t test1 -t test2 -o Model -c JhwContext -f
```

