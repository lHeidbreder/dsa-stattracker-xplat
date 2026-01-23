using System;
using System.Diagnostics;

namespace dsa_battle_tracker;

//TODO: hook in
public class ExceptionHandler : IObserver<Exception>
{
    public static void Init()
    {
        ReactiveUI.RxApp.DefaultExceptionHandler = new ExceptionHandler();
    }
    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached) Debugger.Break();

        _ = Msg.ErrorNotice(value, "OnNext");
    }

    public void OnError(Exception error)
    {
        if (Debugger.IsAttached) Debugger.Break();

        _ = Msg.ErrorNotice(error, "OnError");
    }

    public void OnCompleted()
    {
        if (Debugger.IsAttached) Debugger.Break();
        //RxSchedulers.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
    }
}
