import { AppSetting } from '../_constants/app-setting';


export class CommonSearchView<T>{
    public pageIndex: number = 0;
    public totalItem: number = 0;
    public itemPerPage: number = 0;
    public datas: Array<T> = [];
}

// Job Operation

export class JobOperationSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public user_id: string = "";
    public wc_code: string = "";
    public build_type: string = "";
    public req_date: string = "";;
}

export class JobOperationDetailView<T>{
    public pageIndex: number = 0;
    public itemPerPage: number = 0;
    public totalItem: number = 0;
    public wc_code: string = "";
    public wc_name: string = "";
    public build_type: string = "";
    public dataTotals: Array<T> = [];
}

export class JobOperationDataTotalView {
    public req_date: any = null;
    public total_plan_qty: number = 0;
    public total_cancel_qty: number = 0;
    public total_act_qty: number = 0;
    public total_defect_qty: number = 0;
    public total_diff_qty: number = 0;
    public dataGroups: JobOperationDataGroupView[] = [];
}

export class JobOperationDataGroupView {
    public req_date: any = null;
    public display_group: string = "";
    public display_type: string = "";
    public pdjit_grp: string = "";
    public plan_qty: number = 0;
    public cancel_qty: number = 0;
    public act_qty: number = 0;
    public defect_qty: number = 0;
    public diff_qty: number = 0;

}
// ****** end *******


// Order Summary

export class OrderSummarySearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public build_type: string = "";
    public req_date: any = null;
    public pdjit_grp: string = "";
    public wc_code: string = "";
}

export class OrderSummaryProdDetailView<T>{
    public req_date: any = null;
    public pdjit_grp: string = "";
    public build_type: string = "";
    public total_plan_qty: number = 0;
    public total_actual_qty: number = 0;
    public total_diff_qty: number = 0;
    public productDetail: Array<T> = [];
}

export class OrderSummaryProductDetailView {
    public prod_code: string = "";
    public prod_name: string = "";
    public bar_code: string = "";
    public style: string = "";
    public weight: number = 0;
    public model: string = "";
    public size_name: string = "";
    public spec: string = "";
    public weight_kg: number = 0;
    public plan_qty: number = 0;
    public act_qty: number = 0;
    public diff_qty: number = 0;
    public porDetail: OrderSummaryPorDetailView[] = [];
}

export class OrderSummaryPorDetailView {
    public por_no: string = "";
    public qty_req: number = 0;
    public weight: number = 0;
    public special_order: string = "";
}

export class SpecDrawingView {
    public prod_code: string = "";
    public prod_name: string = "";
    public bar_code: string = "";
    public sd_no: string = "";
    public design_no: string = "";
    public type: string = "";
    public file_path: string = "";
    public design_code: string = "";
    public file_name: string = "";
    public dsgn_no : string = "";
    public dept_code : string = "";
}


// ****** end *******

// Order InFo
export class OrderInfoHeaderView {
    public entity: string = "";
    public entity_name: string = "";
    public por_no: string = "";
    public por_date: any = null;
    public req_date: any = null
    public ord_type: string = "";
    public ord_type_name: string = "";
    public por_type: string = "";
    public por_type_name: string = "";
    public cust_code: string = "";
    public cust_name: string = "";
    public dept: string = "";
    public dept_name: string = "";
    public country: string = "";
    public ref_no: string = "";
    public por_priority: string = "";
    public wh_code: string = "";
    public wh_name: string = "";
    public grp_code: string = "";
    public por_remark: string = "";
    public orderDetail: OrderInFoDetailView[] = [];
    public orderSpecial: OrderSpecialView[] = [];
    public remark: OrderRemarkView[] = [];
}

export class OrderInFoDetailView {
    public por_no: string = "";
    public line_no: number = 0;
    public prod_code: string = "";
    public prod_name: string = "";
    public packaging: string = "";
    public pdgrp_code: string = "";
    public pdcolor_code: string = "";
    public design: string = "";
    public pdsize_code: string = "";
    public uom: string = "";
    public qty_ord: number = 0;
    public qty_act: number = 0;
    public gplabel_no: string = "";
    public skb_flag: string = "";
    public dsgn_no: string = "";
    public sd_no: string = "";
    public bar_code: string = "";
    public subProduct: OrderSubProdDetailView[] = [];
}

export class OrderSubProdDetailView {
    public por_no: string = "";
    public item: number = 0;
    public bom_code: string = "";
    public description: string = "";
    public size: string = "";
    public width: number = 0;
    public length: number = 0;
    public height: number = 0;
    public size_uom: string = "";
    public pack: number = 0;
    public qty_ord: number = 0;
    public uom_code: string = "";
}

export class OrderSpecialView {
    public por_no: string = "";
    public prod_code: string = "";
    public prod_name: string = "";
    public spc_desc: string = "";
}

export class OrderRemarkView {
    public line_no: number = 0;
    public trcmt_desc: string = "";
}


// ****** end *******

export class OrderSummaryParamView {
    public build_type: string = "";
    public pdjit_grp: string = "";
    public pdjit_grp_desc: string = "";
    public req_date: any = null;
    public wc_code: string = "";
    public isEdit: boolean = false;
    public hideSerialNo: boolean = false;
    public isSaleBed: boolean = false;

}

export class OrderDetailParamView {
    public entity_code: string = "";
    public por_no: string = "";
    public: boolean = false;
    public hideSerialNo: boolean = false;
    public isSaleBed: boolean = false;
}

export class SpecDrawingParamView {
    public bar_code: string = "";
    public dsgn_no: string = "";
    public isEdit: boolean = false;
    public hideSerialNo: boolean = false;
    public isSaleBed: boolean = false;
}


export class ProductionTrackingSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public build_type: string = "";
    public req_date: any = null;
    public pdjit_grp: string = "";
    public wc_code: string = "";
    public user_id: string = "";
}

export class ProductionTrackingView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public totalItem: number = 0;
    public entity: string = AppSetting.entity;
    public req_date: any = null;
    public build_type: string = "";
    public productGroups: ProductDataGroupView[] = [];
    public displayGroups: DisplayWcGroupView[] = [];
}

export class ProductDataGroupView {
    public prod_code: string = "";
    public prod_name: string = "";
    public style: string = "";
    public model_name: string = "";
    public size_name: string = "";
    public type : string = "";
    public plan_qty: number = 0;
    public dataGroups: ProductDataGroupDetailView[] = [];
}

export class ProductDataGroupDetailView {
    public wc_code: string = "";
    public wc_name: string = "";
    public qty: number = 0;
}

export class DisplayWcGroupView {
    public wc_code: string = "";
    public wc_name: string = "";

}
