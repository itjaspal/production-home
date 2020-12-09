import { AppSetting } from "../_constants/app-setting";

export class PutAwayDetailSearchView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public doc_code: string = "";
    public set_no: string = "";
    public prod_code: string = "";
    public bar_code: string = "";
}

export class PutAwayTotalView<T>
{
    public total_qty : number = 0;
    public ptwDetails: Array<T> = [];
}

export class PutAwayScanSearchView {
    public entity: string = AppSetting.entity;
    public build_type: string = "";
    public doc_no: string = "";
    public doc_code: string = "";
    public doc_date: string = "";
    public bar_code: string = "";
    public wc_code: string = "";
    public fr_wh_code: string = "";
    public wh_code: string = "";
    public loc_code: string = "";
    public user_id: string = "";
    public qty : number = 0;
}

export class PutAwayScanFinView
{
    public total_qty : number = 0;
    public datas: PutAwayScanView[] = [];
}

export class PutAwayScanView
{
    public item_no : number = 0;
    public set_no : number = 0;
    public prod_code : string = "";  
    public bar_code : string = "";  
    public prod_name : string = "";  
    public wh_code : string = ""; 
    public loc_code : string = ""; 
    public qty : number = 0;

}

export class PutAwayCancelSearchView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public doc_code: string = "";
    public item: string = "";
    public bar_code: string = "";
    public prod_code: string = "";
}


export class WhDefaultView {
    public wh_code: string = "";
}

export class DeptDefaultView {
    public dept_code: string = "";
}

export class VerifyLocView {
    public loc_code: string = "";
}
