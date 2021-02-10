import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material';
import { AppSetting } from '../../_constants/app-setting';
import { CommonSearchView } from '../../_model/common-search-view';
import { UploadFileSearchView, UploadFileView } from '../../_model/upload-file';
import { MessageService } from '../../_service/message.service';
import { UploadFileService } from '../../_service/upload-file.service';

@Component({
  selector: 'app-upload-file-search',
  templateUrl: './upload-file-search.component.html',
  styleUrls: ['./upload-file-search.component.scss']
})
export class UploadFileSearchComponent implements OnInit {

  constructor(
    private _uploadSvc: UploadFileService,
    private _msgSvc: MessageService,
  ) { }

  public toppingList: any = [];
  public model: UploadFileSearchView = new UploadFileSearchView();
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;
  actions: any = {};

  public data: CommonSearchView<UploadFileView> = new CommonSearchView<UploadFileView>();

  ngOnInit() {
  }

  async ngOnDestroy() {
    this.saveSession();
  }
  
  async saveSession() {
    sessionStorage.setItem('session-upload-search', JSON.stringify(this.model));
  }
  
  async search(event: PageEvent = null) {
  
    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }
    console.log(this.model)
  
    this.data = await this._uploadSvc.searchUploadFile(this.model);
    console.log(this.data);
  }
  
  
  
   async delete(file) {
    console.log(file);
    this._msgSvc.confirmPopup("ยืนยันลบข้อมูล", async result => {
      if (result) {
        let res: any = await this._uploadSvc.delete(file);

        this._msgSvc.successPopup(res.message);

        await this.search();
      }
    })

  }

  view(x: UploadFileView) {
    window.open(x.fullPath);
  }
}
