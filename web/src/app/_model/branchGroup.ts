import { AppSetting } from '../_constants/app-setting';

export class BranchGroupSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public branchGroupCode: string = "";
    public branchGroupName: string = "";    
}

export class BranchGroupView {
    public branchGroupId: number = 0;
    public branchGroupCode: string = "";
    public branchGroupName: string = "";   
}
