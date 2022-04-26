namespace TSGSystemsToolkit.CmdLine.Services;

public interface IAuthService
{
    Task<bool> Authenticate(Func<InvocationContext, Task<int>> callback, InvocationContext context, CancellationToken ct = default);
    Task<bool> AuthSimple();
}
