import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ScanApproveSendSearchView, ScanApproveSendView, ScanApproveSendDataView, ScanApproveAddView, ScanApproveView } from '../_model/scan-approve-send';

@Injectable({
  providedIn: 'root'
})
export class ScanApproveSendService {

  constructor(
    private http: HttpClient
  ) { }

  public async searchScanApproveSend(_model: ScanApproveSendSearchView) {
    return await this.http.post<ScanApproveSendView>(environment.API_URL + 'scanapvsend/postSearchScanSend', _model).toPromise();
  }

  public async ScanApproveSendNew(_model: ScanApproveAddView) {
    return await this.http.post<ScanApproveView>(environment.API_URL + 'scanapvsend/postScanApvSendNew', _model).toPromise();
  }
  
}
