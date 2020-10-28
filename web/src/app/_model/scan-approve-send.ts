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