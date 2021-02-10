import { AppSetting } from '../_constants/app-setting';
import { DisplayWcGroupView, ProductDataGroupView } from './job-operation';

export class JobOperationStockSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public user_id: string = "";
    public wc_code: string = "";
    public build_type: string = "";
    public req_date: string = "";
}

export class JobOperationStockView {
    public pageIndex: number = 0;
    public itemPerPage: number = 0;
    public totalItem: number = 0;
    public wc_code: string = "";
    public wc_name: string = "";
    public build_type: string = "";
    public porGroups: PorStockGroupView[] = [];
    public displayGroups: DisplayGroupView[] = [];

}

export class DisplayGroupView {
    public disgroup_code: string = "";
    public disgroup_desc: string = "";
}

export class PorStockGroupView {
    public entity: string = AppSetting.entity;
    public por_no: string = "";
    public ref_no: string = "";
    public design_name: string = "";
    public qty: number = 0;
    public dataGroups: PorStockGroupDetailView[] = [];

}

export class PorStockGroupDetailView {
    public disgroup_code: string = "";
    public disgroup_desc: string = "";
    public qty: string = "";
}


export class PorTypeDetailView {
    public distype_code: string = "";
    public distype_desc: string = "";
    public qty: number = 0;
}

export class ProductGroupSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public user_id: string = "";
    public wc_code: string = "";
    public build_type: string = "";
    public req_date: string = "";
    public por_no: string = "";
    public ref_no: string = "";
}

export class ProductionTrackingStockSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity_code: string = AppSetting.entity;
    public user_id: string = "";
    public wc_code: string = "";
    public build_type: string = "";
    public req_date: any = null;
    public por_no: string = "";
    public ref_no: string = "";
}

export class OrderReqSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public entity: string = AppSetting.entity;
    public por_no: string = "";
    public wc_code: string = "";
    public user_id:string = "";
    public build_type:string = "";
}

export class OrderReqView {
    public por_no: string = "";
    public wc_code: string = "";
    public req_date : any = null
}