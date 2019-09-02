/// A Signal for Starting the Context

using System;
using strange.extensions.signal.impl;
using System.Collections.Generic;

public class LotterySignal : Signal
{

}


public class GetPointSignal : Signal
{

}

public class ZhuiqiListSignal : Signal<List<QihaoObj>>
{

}

public class ZHorderRtnSignal : Signal<int>
{

}


public class LoRecordSignal : Signal<RecordObj>
{

}


public class OrderSuccessSignal : Signal
{
 
}


public class CheDanSuccessSignal : Signal
{

}

public class VisionSignal : Signal
{

}

//��ź�
public class SevenDaySignal : Signal<MSG_GP_USER_HDZX7TLRULERESULT>
{

}

public class ChongZhiSongSignal : Signal<MSG_GP_USER_HDZXXRCZSRULET>
{

}

public class TuiGuangSignal : Signal<MSG_GP_USER_HDZXYJTGJLRUSULT>
{

}

//��Ա�����ź�
public class AddMemberInfoSignal : Signal<MSG_GP_USER_GETPLAYMAXPOINTRESULT>
{

}

public class GetPeiESignal : Signal<MSG_GP_USER_GETPLAYPE>
{

}