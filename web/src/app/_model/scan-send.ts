import { AppSetting } from "../_constants/app-setting";

export class ScanSendView {
    public entity:string = AppSetting.entity;
    public pcs_barcode: string = "";
    public prod_code: string = "";
    public prod_name: string = "";
    public job_no: string = "";
    public set_no: string = "";
    public set_qty: number = 0;
    public req_date: string = "";
    public scan_qty : number = 0;
    public bar_code: string = "";
    public fin_date: string = "";
    public wc_code: string = "";
    public show_qty: string = "";
    public user_id:string = "";
}

export class ScanSendSearchView {
    public entity: string = AppSetting.entity;
    public req_date: any = "";
    public pcs_barcode: string = "";
    public wc_code : string = "";
    public user_id : string = "";   
    public build_type: string = "";
}

export class SetNoSearchView {
    public entity: string = AppSetting.entity;
    public tran_date: string = "";
    public wc_code:string = "";
}

export class SetNoView {
    public set_no: string = "";
    public req_date: string = "";

}

export class ScanSendFinView
{
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem : number = 0;
    public datas: ScanSendFinDataView[] = [];
}

export class ScanSendFinDataView
{
    public pcs_barcode: string = "";
    public prod_code: string = "";
    public prod_name: string = "";
    public job_no: string = "";
    public req_date: string = "";
    public wc_code: string = "";
    public entity:string= AppSetting.entity;
}

export class PrintSetNoView {
    public entity :string= AppSetting.entity;
    public set_no: string = "";
    public req_date: string = "";
    public wc_code : string ="";
    public scan_qty : number = 0;
    public set_qty : number = 0;
    public user_id : string = "";

}
