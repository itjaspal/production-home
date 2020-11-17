import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PrintInProcessTagStockView } from '../_model/print-inprocess-tag-stock';

@Injectable({
  providedIn: 'root'
})
export class PrintInprocessTagStockService {

  constructor(
    private http: HttpClient
  ) { }
  public async PringTagStock(_model: PrintInProcessTagStockView) {
    return await this.http.post(environment.API_URL + 'print-inprocess-tag-stock/postPrintTagStock', _model).toPromise();  
  }
}
