import { AppSetting } from "../_constants/app-setting";

export class ScanReceiveDataView {
    public pageIndex: number;
    public totalItem: number;
    public itemPerPage: number;
    public entity: string = AppSetting.entity;
    public build_type: string = "";
    public total_qty_pdt: number = 0;
    public total_qty_rec: number = 0;
    public datas: ScanReceiveDataDetailView[] = [];

}

export class ScanReceiveDataDetailView {
    public entity: string = AppSetting.entity;
    public req_date: string = "";
    public doc_no: string = "";
    public wc_code: string = "";
    public gen_by: string = "";
    public gen_date: string = "";
    public qty_pdt: number = 0;
    public qty_rec: number = 0;
    public doc_status: string = "";
    public user_id: string = "";
}

export class ScanReceiveDataSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public doc_date: string = "";
    public send_type: string = "";
    public build_type: string = "";
    public user_id: string = "";
}

export class ScanReceiveSearchView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public scan_type: string = "";
    public scan_data: string = "";
    public scan_qty: number = 1;
    public user_id: string = "";
    public build_type: string = "";
}

export class ScanReceiveView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public set_no: string = "";
    public line_no: number = 0;
    public prod_code: string = "";
    public qty: number = 0;
}

export class ScanReceiveFinView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem: number = 0;
    public datas: ScanReceiveFinDataView[] = [];
}

export class ScanReceiveFinDataView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public set_no: string = "";
    public line_no: number = 0;
    public prod_code: string = "";
    public qty: number = 0;
}

export class SendProductSearch {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
}

export class ConfirmStockDataView {
    public entity: string = AppSetting.entity;
    public doc_no: string = "";
    public user_id: string = "";
}

export class ScanCheckQrSearchView {
    public entity: string = AppSetting.entity;
    public qr: string = "";
   
}

export class ScanCheckQrView {
    public prod_code: string = "";
    public prod_name: string = "";
    public design_name : string = ""; 
    public req_date : string = "";
    public doc_no : string = "";
    public doc_date : string = "";
    public set_no : string = ""; 
}

export class ScanCheckQrFinView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem: number = 0;
    public datas: ScanCheckQrFinDataView[] = [];
}

export class ScanCheckQrFinDataView {
    public prod_code: string = "";
    public prod_name: string = "";
    public design_name : string = ""; 
    public req_date : string = "";
    public doc_no : string = "";
    public doc_date : string = "";
    public set_no : string = ""; 
}

