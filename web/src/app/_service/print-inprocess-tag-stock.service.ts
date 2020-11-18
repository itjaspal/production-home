import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PrintInProcessTagStockView, TagProductStockSearchView, TagStockGroupView, TagStockProductView } from '../_model/print-inprocess-tag-stock';

@Injectable({
  providedIn: 'root'
})
export class PrintInprocessTagStockService {

  constructor(
    private http: HttpClient
  ) { }

  public async getGroup(_model: TagProductStockSearchView) {
    return await this.http.post<TagStockGroupView[]>(environment.API_URL + 'print-inproces-tag-stock/postGetGroup', _model).toPromise();
  }

  public async getProduct(_model: TagProductStockSearchView) {
    return await this.http.post<TagStockProductView[]>(environment.API_URL + 'print-inproces-tag-stock/postGetProduct', _model).toPromise();
  }

  public async PringTagStock(_model: PrintInProcessTagStockView) {
    return await this.http.post(environment.API_URL + 'print-inproces-tag-stock/postPrintTagStock', _model).toPromise();  
  }
}
