import { AppSetting } from '../_constants/app-setting';

export class ScanDefectView {
    public entity: string = AppSetting.entity;
    public sub_prod_code: string = "";
    public sub_prod_name: string = "";
    public prod_code: string = "";
    public prod_name: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public qty_plan: number = 0;
    public qty_fin: number = 0;
    public qty: number = 0;
    public wc_code: string = "";
    public item_no: number = 0;
}

export class ScanDefectSearchView {
    public entity: string = AppSetting.entity;
    public build_type: string = "";
    public req_date: string = "";
    public scan_data: string = "";
    public prod_code: string = "";
    public sub_prod_code: string = "";
    public wc_code: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public user_id: string = "";
    public qty: number = 0;
    public item_no :number = 0;
}

export class ScanDefectScanFinView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem: number = 0;
    public datas: ScanDefectScanDataView[] = [];
}

export class ScanDefectScanDataView {
    public sub_prod_code: string = "";
    public sub_prod_name: string = "";
    public prod_code: string = "";
    public entity: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public qty: number = 0;
    public wc_code: string = "";
    public item_no: number = 0;
}

export class DefectProductSubSearchView {
    public entity: string = AppSetting.entity;
    public req_date: string = "";
    public por_no: string = "";
    public ref_no: string = "";
    public wc_code: string = "";
    public item_no : number = 0;
}

export class DefectProductSubView {
    public prod_code: string = "";
    public sub_prod_code: string = "";
    public sub_prod_name: string = "";
    public qty_plan: number = 0;
    public qty_fin: number = 0;
    public item_no: number = 0;
}