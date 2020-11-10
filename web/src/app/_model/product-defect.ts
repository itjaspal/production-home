import { AppSetting } from "../_constants/app-setting";

export class ProductDefectView {
    public pageIndex: number;
    public totalItem: number;
    public itemPerPage: number;
    public entity: string = AppSetting.entity;
    public req_date: Date = null;
    public build_type: string = "";
    public total_qty_pdt: number = 0;
    public total_qty_cutting: number = 0;
    public total_qty_wip: number = 0;
    public total_qty_fin: number = 0;
    public datas: ProductDefectDetailView[] = [];
}

export class ProductDefectDetailView {
    public entity: string = AppSetting.entity;
    public por_no: string = "";
    public ref_no: string = "";
    public prod_code: string = "";
    public prod_name: string = "";
    public brand_name: string = "";
    public design_name: string = "";
    public size_name: string = "";
    public qty_pdt: number = 0;
    public qty_cutting: number = 0;
    public qty_wip: number = 0;
    public qty_fin: number = 0;
}

export class ProductDefectSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity: string = AppSetting.entity;
    public por_no: string = "";
    public req_date: string = "";
    public build_type: string = "";
    public user_id: string = "";
}

export class DataQcView{
    public entity : string = AppSetting.entity;
    public por_no : string = "";
    public ref_no : string = "";
    public prod_code : string = "";
    public size_name : string = "";
    public qc_qty : number =0;
}

export class DataQcCuttingView
{
    public entity : string = AppSetting.entity;
    public por_no : string = "";
    public ref_no : string = "";
    public prod_code : string = "";
    public build_type : string = "";
    public qc_date : string ="";
    public qc_process : string = "";
    public item_no : number = 0;
    public qc_qty : number = 0;
    public no_pass_qty : number = 0;
    public width : string = "";
    public lenght : string = "";
    public remark1 : string = "";
    public remark2 : string = "";
    public remark3 : string = "";
    public user_id : string = "";
    public size_name : string = "";
    public qc_time : string = "";
}

export class ItemNoSearchView
{
    public entity : string = AppSetting.entity;
    public por_no : string = "";
}

export class ItemNoListView
{
    public item_no : number = 0;
}