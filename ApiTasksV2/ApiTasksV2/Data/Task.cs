using Dapper.Contrib.Extensions;

namespace ApiTasksV2.Data;

[Table("tasks")]
public record Task (int id, string activity, string status);
