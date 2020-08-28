import { AppSetting } from '../_constants/app-setting';

export class BranchSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public branchDesc: string = "";
    public entityCode: string = "";
    public branchGroupId: number = undefined;
    public branchGroupIdList: any = [];
    public branchId : number = undefined;
    public status: any = [];
}

export class BranchView {
    public branchId: number = undefined;
    public branchCode: string = "";
    public branchName: string = "";
    public branchNameThai: string = "";
    public branchNameEng: string = "";
    public branchGroupId: number = undefined;
    public branchGroupCode: string = "";
    public branchGroupName: string = "";
    public entityCode: string = "";
    public status: string = "A";
    public email: string = "";
    public docRunningPrefix: string = "";


    public statusTxt: string = "";
}