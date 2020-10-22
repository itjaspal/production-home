import { SetNoSearchView, SetNoView } from './../_model/scan-send';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ScanSendSearchView, ScanSendView } from '../_model/scan-send';

@Injectable({
  providedIn: 'root'
})
export class ScanSendService {

  constructor(
    private http: HttpClient
  ) { }

  public async scansendadd(_model: ScanSendSearchView) {
    return await this.http.post<ScanSendView>(environment.API_URL + 'scan-send/postScanSendAdd', _model).toPromise();
  }

  public async searchsetno(_model: SetNoSearchView) {
    return await this.http.post<SetNoView>(environment.API_URL + 'scan-send/postSearchSetNo', _model).toPromise();
  }

  public async PrintSticker(_model: ScanSendView) {
    return await this.http.post(environment.API_URL + 'scan-send/postPrintSticker', _model).toPromise();  
  }

  public async delete(params) {
    return await this.http.post(environment.API_URL + 'scan-send/post/Delete',params).toPromise();
  }

  public async getscanqty(_model: ScanSendView) {
    return await this.http.post<SetNoView>(environment.API_URL + 'scan-send/postGetScanQty', _model).toPromise();
  }


}
