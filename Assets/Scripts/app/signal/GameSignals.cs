/// A Signal for Starting the Context

using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;


public class GongGaoSignal : Signal<int,string>
{

}

//�����ʽ� �ź�
public class GeRenzijiSignal : Signal<MSG_GP_USER_GETBACKWDZHJBXX>
{

}
//�Ŷ��ʽ� �ź�
public class TuanduizijiSignal : Signal<MSG_GP_TEAM_GETBACKJBXX>
{

}

//�޸�QQ/����/ȡ������ �ź�
public class QQMiMaQuKuanMiMaSignal : Signal<MSG_GP_USER_ChangeUserPassWordResult>
{

}

public class GetUserinfoSignal : Signal
{

}

public class GetAllbankinfoSignal : Signal<List<GetBankInfo>>
{

}

public class RefreshMoneySignal : Signal
{

}

public class TransferSignal : Signal
{

}

public class TouzhuXiangxiSignal : Signal
{

}

public class RecordBackSignal : Signal<RecordBackObj>
{
 
}
	
public class QpGameInfoSignal: Signal<List<ComNameInfo>>
{

}

public class QpRoomInfoSignal: Signal<List<ComRoomInfo>>
{

}