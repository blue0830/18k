/// If you're new to Strange, start with MyFirstProject.
/// If you're interested in how Signals work, return here once you understand the
/// rest of Strange. This example shows how Signals differ from the default
/// EventDispatcher.
/// All comments from MyFirstProjectContext have been removed and replaced by comments focusing
/// on the differences 

using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;


public class LotteryContext : MVCSContext
{
    public LotteryContext(MonoBehaviour view) : base(view)
    {
    }

    public LotteryContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }

    // Override Start so that we can fire the StartSignal 
    override public IContext Start()
    {
        base.Start();
        StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
        startSignal.Dispatch();
        return this;
    }

    protected override void mapBindings()
    {
        //model
        injectionBinder.Bind<IUInfoModel>().To<UInfoModel>().ToSingleton();
        injectionBinder.Bind<IConfigModel>().To<ConfigModel>().ToSingleton();
        injectionBinder.Bind<ILotteryModel>().To<LotteryModel>().ToSingleton();
		injectionBinder.Bind<IGameInfoModel>().To<GameInfoModel>().ToSingleton();
		//百家乐
		injectionBinder.Bind<IBaccaratInfoModel>().To<BaccaratInfoModel>().ToSingleton();

        //UI
        mediationBinder.Bind<LoginView>().To<LoginMediator>();
        mediationBinder.Bind<MessageView>().To<MessageMediator>();
        mediationBinder.Bind<MainView>().To<MainMediator>();

        mediationBinder.Bind<MemberView>().To<MemberMediator>();
        mediationBinder.Bind<ActivityView>().To<ActivityMediator>();
        mediationBinder.Bind<RecordView>().To<RecordMediator>();
        mediationBinder.Bind<UserView>().To<UserMediator>();

        mediationBinder.Bind<SelecterView>().To<SelecterMediator>();
        mediationBinder.Bind<SelectionConfirmView>().To<SelectionConfirmMediator>();

        //commandBinder.Bind<CallWebServiceSignal>().To<CallWebServiceCommand>();

        //StartSignal is now fired instead of the START event.
        //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
        commandBinder.Bind<StartSignal>().To<StartCommand>().Once();
 
       
        //signal
        injectionBinder.Bind<LoginSignal>().ToSingleton();
        injectionBinder.Bind<MsgSignal>().ToSingleton();
        injectionBinder.Bind<DeleteCfirmPSignal>().ToSingleton();
        injectionBinder.Bind<ShowCfirmPSignal>().ToSingleton();
        injectionBinder.Bind<LotterySignal>().ToSingleton();
        injectionBinder.Bind<GetPointSignal>().ToSingleton();
        injectionBinder.Bind<ZhuiqiListSignal>().ToSingleton();
        injectionBinder.Bind<ZHorderRtnSignal>().ToSingleton();
        injectionBinder.Bind<LoRecordSignal>().ToSingleton();
        injectionBinder.Bind<GoSelectViewSignal>().ToSingleton();
        injectionBinder.Bind<OrderSuccessSignal>().ToSingleton();
		injectionBinder.Bind<LogOutSignal>().ToSingleton();

        //公告
        injectionBinder.Bind<GongGaoSignal>().ToSingleton();

        injectionBinder.Bind<GeRenzijiSignal>().ToSingleton();
        injectionBinder.Bind<TuanduizijiSignal>().ToSingleton();
        injectionBinder.Bind<GetUserinfoSignal>().ToSingleton();
        injectionBinder.Bind<GetAllbankinfoSignal>().ToSingleton();
        injectionBinder.Bind<RefreshMoneySignal>().ToSingleton();
		injectionBinder.Bind<TransferSignal>().ToSingleton();
        injectionBinder.Bind<TouzhuXiangxiSignal>().ToSingleton();
 

        injectionBinder.Bind<QQMiMaQuKuanMiMaSignal>().ToSingleton();
        injectionBinder.Bind<RecordBackSignal>().ToSingleton();
        injectionBinder.Bind<CheDanSuccessSignal>().ToSingleton();
        injectionBinder.Bind<VisionSignal>().ToSingleton();


        injectionBinder.Bind<SevenDaySignal>().ToSingleton();
        injectionBinder.Bind<ChongZhiSongSignal>().ToSingleton();
        injectionBinder.Bind<TuiGuangSignal>().ToSingleton();

        injectionBinder.Bind<AddMemberInfoSignal>().ToSingleton();
        injectionBinder.Bind<GetPeiESignal>().ToSingleton();
		injectionBinder.Bind<QpGameInfoSignal>().ToSingleton();
		injectionBinder.Bind<QpRoomInfoSignal>().ToSingleton();
    }
}