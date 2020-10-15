import { AppSetting } from "../_constants/app-setting";

export class PrintInProcessTagView 
{
    public prod_code: string = "";
    public bar_code: string  = "";
    public prod_name:string  = "";
    public pcs_barcode:string  = "";
    public qty:number = 0;
    public description : string = "";
    public req_date : string = "";
    public user_id : string = "";
    public size_name : string = "";
    public design_name : string = "";
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

export class TagProductSearchView
{
    public entity : string = AppSetting.entity;
    public req_date : any = null;
    public wc_code : string = "";
    public pdjit_grp: string = "";
    public bar_code : string = "";
}

export class TagProductView
{
    public prod_code : string = "";
    public bar_code : string = "";
    public prod_name : string = "";
}