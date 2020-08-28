import { AppSetting } from '../_constants/app-setting';

export class DepartmentSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public departmentCode: string = "";
    public departmentName: string = "";    
    public status: string ="";
}

export class DepartmentView {
    public departmentId: number = 0;
    public departmentCode: string = "";
    public departmentName: string = "";   
    public status: string = "A";
}
