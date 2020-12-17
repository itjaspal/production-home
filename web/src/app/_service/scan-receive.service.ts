import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ScanReceiveDataSearchView, ScanReceiveDataView, ScanReceiveSearchView, ScanReceiveView } from '../_model/scan-receive';

@Injectable({
  providedIn: 'root'
})
export class ScanReceiveService {

  constructor(
    private http: HttpClient
  ) { }

  public async searchDataScanReceive(_model: ScanReceiveDataSearchView) {
    return await this.http.post<ScanReceiveDataView>(environment.API_URL + 'scan-receive/postSearchDataScanReceive', _model).toPromise();
  }

  public async ScanReceiveNew(_model: ScanReceiveSearchView) {
    return await this.http.post<ScanReceiveView>(environment.API_URL + 'scan-receive/postScanReceiveAdd', _model).toPromise();
  }

  public async ScanReceiveCancel(params) {
    return await this.http.post(environment.API_URL + 'scan-receive/postScanReceiveCancel',params).toPromise();
  }

  public async getProductDetail(_entity: string , _docNo: string) {
    return await this.http.get(environment.API_URL + 'scan-receive/getProductDetail/' + _entity+'/'+_docNo).toPromise(); 

  } 

  public async SendMail(_entity: string , _docNo: string) {
    return await this.http.get(environment.API_URL + 'scan-receive/getSendMail/' + _entity+'/'+_docNo).toPromise(); 

  } 
}
