export class CommonSearchView<T>{
    public pageIndex: number = 0;
    public totalItem: number = 0;
    public itemPerPage: number = 0;
    public total_plan_qty: number = 0;
    public total_actual_qty: number = 0;
    public total_diff_qty: number = 0;
    public datas: Array<T> = [];
} 

