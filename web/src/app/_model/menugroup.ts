import { AppSetting } from '../_constants/app-setting';

export class MenuGroupSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public menuFunctionGroupId: string = "";
    public menuFunctionGroupName: string = ""; 
    public iconName: string = "";      
    public orderDisplay : number = 0;
}

export class MenuGroupView {
    public menuFunctionGroupId: string = "";
    public menuFunctionGroupName: string = "";
    public iconName: string = "";   
    public orderDisplay : number = 0;
    public menuGroup : string = "";
}
