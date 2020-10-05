import { AppSetting } from '../_constants/app-setting';

export class JobOperationSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public user_id: string = "";
    public wc_code: string = "";
    public build_type: string = "";
    public req_date: string = "";;
}

export class CommonSearchView<T>{
    public pageIndex: number = 0;
    public totalItem: number = 0;
    public itemPerPage: number = 0;
    public datas: Array<T> = [];
}


export class JobOperationDetailView<T>{ 
    public pageIndex: number = 0;
    public itemPerPage: number = 0;
    public totalItem: number = 0;
    public wc_code: string = "";
    public wc_name: string = "";
    public build_type: string = "";
    public dataTotals: Array<T> = [];
    //public dataTotals: JobOperationDataTotalView[] = [];  
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

export class JobOperationDataGroupView{ 
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



