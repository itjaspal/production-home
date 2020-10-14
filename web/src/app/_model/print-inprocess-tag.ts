import { AppSetting } from "../_constants/app-setting";

export class PrintInProcessTagView 
{
    public prod_code: string = "";
    public bar_code: string  = "";
    public prod_name:string  = "";
    public pcs_barcode:string  = "";
    public qty:number = 0;
    public description : string = "";
}

export class PrintInProcessTagSearchView
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