import { AppSetting } from "../_constants/app-setting";

export class MenuSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public menuFunctionId: string = "";
    public menuFunctionGroupId: string = "";    
    public menuFunctionName: string ="";
    public menuURL : string = "";
    public iconName: string ="";
    public orderDisplay: number = 0;
    
}

export class MenuView {
    public menuFunctionId: string = "";
    public menuFunctionGroupId: string = "";    
    public menuFunctionName: string ="";
    public menuURL : string = "";
    public iconName: string ="";
    public orderDisplay: number = 0;
}
