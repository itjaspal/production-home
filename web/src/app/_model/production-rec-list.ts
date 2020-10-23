import { AppSetting } from '../_constants/app-setting';

// production receiving list

export class productionRecListSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public build_type: string = "";
    public doc_date: string = "";;
}

export class productionRecListTotalView<T>{ 
    public pageIndex: number = 0;
    public itemPerPage: number = 0;
    public totalItem: number = 0;
    public total_rec_qty: number = 0;
    public recDetails: Array<T> = [];
}

export class productionRecListDetailView{ 
    public jit_date: any = null;
    public doc_no: string = "";
    public wc_code: string = "";
    public wc_name: string = "";
    public gen_date: any = null;
    public gen_by: string = "";
    public conf_qty: number = 0;
}

// production receiving list Detail

export class productionRecListDetailSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public doc_date: string = "";
    public doc_no: string = "";
    public build_type: string = "";
}

export class productionRecListDetailTotalView<T>{ 
    public pageIndex: number = 0;
    public itemPerPage: number = 0;
    public totalItem: number = 0;
    public total_rec_qty: number = 0;
    public total_prod_item: number = 0;
    public recDetails: Array<T> = [];
}

export class productionRecListDetailProdView {
    public doc_no: string = "";
    public wc_code: string = "";
    public prod_code: string = "";
    public prod_tname: string = "";
    public qty_pdt: number = 0;
    public por_no: string = "";
    public uom_code: string = "";
    public mps_date: any = null;
    public setDetails: productionRecListSetDetailView[] = [];
}

export class productionRecListSetDetailView{ 
     public pkg_barcode_set: number = 0;
     public confirm_qty: number = 0;
}

export class productionRecListParamView {
    public doc_no: string = "";
    public doc_date: string = "";
    public isEdit: boolean = false;
    public hideSerialNo: boolean = false;
    public isSaleBed: boolean = false;
}