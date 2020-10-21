import { AppSetting } from "../_constants/app-setting";

export class ScanSendView {
    public pcs_barcode: string = "";
    public prod_code: string = "";
    public prod_name: string = "";
    public job_no: string = "";
    public set_no: string = "";
    public set_qty: number = 0;
    public req_date: string = "";
}

export class ScanSendSearchView {
    public entity: string = AppSetting.entity;
    public req_date: string = "";
    public psc_barcode: string = "";
}

export class SetNoSearchView {
    public entity: string = AppSetting.entity;
    public req_date: string = "";
}

export class SetNoView {
    public set_no: string = "";
    public req_date: string = "";

}
