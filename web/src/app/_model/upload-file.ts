import { AppSetting } from '../_constants/app-setting';

export class UploadFileView {
    public pddsgn_code: string = "";
    public pddsgn_name: string = "";
    public type : string = "";
    public file_path: string = "";
    public file_name: string = "";
    public fullPath: string = "";

    public file:any = null;
}

export class UploadFileSearchView {
    public pageIndex: number = 1;
    public itemPerPage: number = AppSetting.itemPerPage;
    public pddsgn_code: string = "";
    public type: string = "";    
}

