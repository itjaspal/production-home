import { AppSetting } from "../_constants/app-setting";

export class JobInProcessView 
{
    public prod_code: string = "";
    public bar_code: string  = "";
    public prod_name:string  = "";
    public pcs_barcode:string  = "";
    public qty:number = 0;
}

export class JobInProcessSearchView
{
    public entity : string = AppSetting.entity;
    public build_type : string = "";
    public req_date : string = "";    
    public pdjit_grp : string = "";
    public bar_code : string = "";
    public wc_code : string = "";
    public user_id : string = "";
    public qty : number = 0;

}

export class JobInProcessScanFinView
{
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem : number = 0;
    public datas: JobInProcessScanView[] = [];
}

export class JobInProcessScanView
{
    //public pcs_barcode : string = "";  
    //public pdmodel_code : string = "";  
    public prod_code : string = "";  
    public prod_name : string = "";  
    public qty : number = 0;
}

export class ProductSearchView
{
    public entity : string = AppSetting.entity;
    public req_date : any = null;
    public wc_code : string = "";
    public pdjit_grp: string = "";
}