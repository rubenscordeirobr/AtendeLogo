//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using StackExchange.Redis;
//using StackExchange.Redis.Maintenance;
//using StackExchange.Redis.Profiling;

//namespace AtendeLogo.Application.UnitTests.Mocks;

//public class ConnectionMultiplexerMock : IConnectionMultiplexer
//{
//    // Events
//    public event EventHandler<RedisErrorEventArgs> ErrorMessage = delegate { };
//    public event EventHandler<ConnectionFailedEventArgs> ConnectionFailed = delegate { };
//    public event EventHandler<InternalErrorEventArgs> InternalError = delegate { };
//    public event EventHandler<ConnectionFailedEventArgs> ConnectionRestored = delegate { };
//    public event EventHandler<EndPointEventArgs> ConfigurationChanged = delegate { };
//    public event EventHandler<EndPointEventArgs> ConfigurationChangedBroadcast = delegate { };
//    public event EventHandler<ServerMaintenanceEvent> ServerMaintenanceEvent = delegate { };
//    public event EventHandler<HashSlotMovedEventArgs> HashSlotMoved = delegate { };

//    // Properties
//    public string ClientName => "MockRedis";
//    public string Configuration => "localhost:6379";
//    public int TimeoutMilliseconds => 1000;
//    public long OperationCount => 0;

//    private bool _preserveAsyncOrder;
//    public bool PreserveAsyncOrder
//    {
//        get => _preserveAsyncOrder;
//        set => _preserveAsyncOrder = value;
//    }

//    public bool IsConnected => true;
//    public bool IsConnecting => false;

//    private bool _includeDetailInExceptions;
//    public bool IncludeDetailInExceptions
//    {
//        get => _includeDetailInExceptions;
//        set => _includeDetailInExceptions = value;
//    }

//    private int _stormLogThreshold;
//    public int StormLogThreshold
//    {
//        get => _stormLogThreshold;
//        set => _stormLogThreshold = value;
//    }

//    public void RegisterProfiler(Func<ProfilingSession?> profilingSessionProvider)
//    {
//        // No profiling in the mock.
//    }

//    public ServerCounters GetCounters()
//    {
//        return new ServerCounters(null);
//    }

//    public EndPoint[] GetEndPoints(bool configuredOnly = false)
//    {
//        return Array.Empty<EndPoint>();
//    }

//    public void Wait(Task task)
//    {
//        task.Wait();
//    }

//    public T Wait<T>(Task<T> task)
//    {
//        return task.GetAwaiter().GetResult();
//    }

//    public void WaitAll(params Task[] tasks)
//    {
//        Task.WaitAll(tasks);
//    }

//    public int HashSlot(RedisKey key)
//    {
//        return 0;
//    }

//    public ISubscriber GetSubscriber(object? asyncState = null)
//    {
//        return new SubscriberMock();
//    }

//    public IDatabase GetDatabase(int db = -1, object? asyncState = null)
//    {
//        return new DatabaseMock();
//    }

//    public IServer GetServer(string host, int port, object? asyncState = null)
//    {
//        return new ServerMock();
//    }

//    public IServer GetServer(string hostAndPort, object? asyncState = null)
//    {
//        return new ServerMock();
//    }

//    public IServer GetServer(IPAddress host, int port)
//    {
//        return new ServerMock();
//    }

//    public IServer GetServer(EndPoint endpoint, object? asyncState = null)
//    {
//        return new ServerMock();
//    }

//    public IServer[] GetServers()
//    {
//        return new IServer[] { new ServerMock() };
//    }

//    public Task<bool> ConfigureAsync(TextWriter? log = null)
//    {
//        return Task.FromResult(true);
//    }

//    public bool Configure(TextWriter? log = null)
//    {
//        return true;
//    }

//    public string GetStatus()
//    {
//        return "Mocked Status";
//    }

//    public void GetStatus(TextWriter log)
//    {
//        log.Write("Mocked Status");
//    }

//    public void Close(bool allowCommandsToComplete = true)
//    {
//        // Do nothing.
//    }

//    public Task CloseAsync(bool allowCommandsToComplete = true)
//    {
//        return Task.CompletedTask;
//    }

//    public string? GetStormLog()
//    {
//        return string.Empty;
//    }

//    public void ResetStormLog()
//    {
//        // Do nothing.
//    }

//    public long PublishReconfigure(CommandFlags flags = CommandFlags.None)
//    {
//        return 0;
//    }

//    public Task<long> PublishReconfigureAsync(CommandFlags flags = CommandFlags.None)
//    {
//        return Task.FromResult(0L);
//    }

//    public int GetHashSlot(RedisKey key)
//    {
//        return 0;
//    }

//    public void ExportConfiguration(Stream destination, ExportOptions options = (ExportOptions)(-1))
//    {
//        // Do nothing.
//    }

//    public void AddLibraryNameSuffix(string suffix)
//    {
//        // Do nothing.
//    }

//    public void Dispose()
//    {
//        // Do nothing.
//    }

//    public ValueTask DisposeAsync()
//    {
//        return ValueTask.CompletedTask;
//    }
//}

//public class DatabaseMock : IDatabase
//{
//    // A simple in-memory key/value store for basic string operations.
//    private readonly Dictionary<RedisKey, RedisValue> _store = new Dictionary<RedisKey, RedisValue>();

//    public int Database => 0;

//    public RedisValue this[RedisKey key]
//    {
//        get => StringGet(key);
//        set => StringSet(key, value);
//    }

//    public bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
//    {
//        _store[key] = value;
//        return true;
//    }

//    public RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None)
//    {
//        return _store.TryGetValue(key, out var value) ? value : RedisValue.Null;
//    }

//    // For brevity, other members are not implemented.
//    public bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public long KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public bool KeyExpire(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public bool KeyExpire(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public TimeSpan KeyTimeToLive(RedisKey key, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public RedisType KeyType(RedisKey key, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public bool HashSet(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public RedisValue HashGet(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public bool SetAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public long SetAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public bool SetRemove(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public long SetRemove(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public RedisValue Execute(string command, params object[] args) => throw new NotImplementedException();
//    public Task<RedisValue> ExecuteAsync(string command, params object[] args) => throw new NotImplementedException();
//    public IBatch CreateBatch(object? state = null) => throw new NotImplementedException();
//    public ITransaction CreateTransaction(object? state = null) => throw new NotImplementedException();
//    public Task<bool> KeyRenameAsync(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    public IConnectionMultiplexer Multiplexer => throw new NotImplementedException();
//    public Task<RedisValue[]> HashMemberStringsAsync(RedisKey key, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();
//    // The remaining members can be stubbed out similarly.
//}

//public class SubscriberMock : ISubscriber
//{
//    public bool IsConnected => true;

//    public long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
//    {
//        // No actual pub/sub behavior.
//        return 0;
//    }

//    public Task<long> PublishAsync(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
//    {
//        return Task.FromResult(0L);
//    }

//    public ChannelMessageQueue Subscribe(RedisChannel channel, CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<ChannelMessageQueue> SubscribeAsync(RedisChannel channel, CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public void Unsubscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public Task UnsubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public void Unsubscribe(RedisChannel channel, CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public Task UnsubscribeAsync(RedisChannel channel, CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public void UnsubscribeAll(CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }

//    public Task UnsubscribeAllAsync(CommandFlags flags = CommandFlags.None)
//    {
//        throw new NotImplementedException();
//    }
//}

//public class ServerMock : IServer
//{
//    public EndPoint EndPoint => new IPEndPoint(IPAddress.Loopback, 6379);
//    public string Host => "localhost";
//    public int Port => 6379;
//    public bool IsConnected => true;
//    public bool AllowConnect => true;
//    public int Version => 1;
//    public int DatabaseCount => 1;
//    public ServerType ServerType => ServerType.Standalone;
//    public bool IsReplica => false;
//    public bool IsSlave => false;
//    public bool IsMaster => true;
//    public bool IsTieBreaker => false;

//    public TimeSpan Ping(TimeSpan timeout = default, CommandFlags flags = CommandFlags.None)
//    {
//        return TimeSpan.FromMilliseconds(1);
//    }

//    public string Execute(string command, params object[] args)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<RedisResult> ExecuteAsync(string command, params object[] args)
//    {
//        throw new NotImplementedException();
//    }

//    public IEnumerable<RedisKey> Keys(int database, RedisValue pattern, int pageSize, long cursor, int pageOffset, CommandFlags flags = CommandFlags.None)
//    {
//        return Enumerable.Empty<RedisKey>();
//    }

//    public ServerCounters GetCounters(CommandFlags flags = CommandFlags.None)
//    {
//        return new ServerCounters();
//    }

//    public Task<ServerCounters> GetCountersAsync(CommandFlags flags = CommandFlags.None)
//    {
//        return Task.FromResult(new ServerCounters());
//    }

//    public IServerClassification Classification => throw new NotImplementedException();
//    public string NodeId => "MockNode";
//    // Other members can be stubbed out as needed.
//}
