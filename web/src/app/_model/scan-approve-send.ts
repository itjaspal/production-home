import { AppSetting } from "../_constants/app-setting";

export class ScanApproveSendView {
    public entity: string = AppSetting.entity;
    public build_type: string = "";
    public user_id: string = "";
    public total_set: number = 0;
    public total_qty: number = 0;
    public datas: ScanApproveSendDataView[] = [];

}

export class ScanApproveSendDataView {
    public req_date: Date;
    public pdjit_grp: string = "";
    public wc_code: string = "";
    public doc_no: string = "";
    public set_qty: number = 0;
    public tot_qty: number = 0;
    public status: string = "";
    public doc_status: string = "";
}

export class ScanApproveSendSearchView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public fin_date: string = "";
    public send_type: string = "";
    public build_type: string = "";
    public user_id: string = "";
}

export class ScanApproveAddView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public set_no: string = "";
    public build_type: string = "";
    public fin_date: string = "";
    public user_id: string = "";
    public wc_code: string = "";
}

export class ScanApproveView {
    public entity : string = AppSetting.entity;
    public doc_no : string = "";  
    public set_no : number = 0;  
    public qty : number = 0;
    public prod_code : string = "";  
    public prod_name : string = "";  
    public fin_date : string = "";  
    public build_type: string = "";
    public wc_code: string = "";
}

export class ScanApproveFinView
{
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem : number = 0;
    public datas: ScanApproveFinDataView[] = [];
}

export class ScanApproveFinDataView
{
    public doc_no : string = "";  
    public set_no : number = 0;  
    public qty : number = 0;
    public prod_code : string = "";  
    public prod_name : string = "";  
    public fin_date : string = "";  
}