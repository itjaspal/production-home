import { AppSetting } from "../_constants/app-setting";

export class TagProductStockSearchView {
    public entity: string = AppSetting.entity;
    public req_date: string = "";
    public wc_code: string = "";
    public por_no: string = "";
    public ref_no: string = "";

}

export class PrintInProcessTagStockView {
    public prod_code: string = "";
    public sub_prod_code: string = "";
    public sub_prod_name: string = "";
    public qty: number = 0;
    public description: string = "";
    public req_date: string = "";
    public user_id: string = "";
    public size_name: string = "";
    public design_name: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public build_no: number = 0;
}

export class PrintInProcessTagStockSearchView {
    public entity: string = AppSetting.entity;
    public build_type: string = "";
    public req_date: string = "";
    public prod_code: string = "";
    public sub_prod_code: string = "";
    public group_line: string = "";
    public wc_code: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public user_id: string = "";
    public qty: number = 0;

}

export class TagStockGroupView {
    public group_line: string = "";
    public group_line_name: string = "";

}

export class TagStockProductView {
    public prod_code: string = "";
    public sub_prod_code: string = "";
    public sub_prod_name: string = "";
    public por_no: string = "";
    public size_name: string = "";
    public design_name: string = "";
    public ref_no: string = "";
    public req_date: string = "";
    public build_no: number = 0;
}

export class TagStockProductSearchView {
    public entity: string = AppSetting.entity;
    public req_date: string = "";
    public wc_code: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public group_line: string = "";
}