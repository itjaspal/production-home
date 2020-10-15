import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PrintInProcessTagView, TagProductSearchView, TagProductView } from '../_model/print-inprocess-tag';

@Injectable({
  providedIn: 'root'
})
export class PrintInprocessTagService {

  constructor(
    private http: HttpClient
  ) { }


  public async getproduct(_model: TagProductSearchView) {
    return await this.http.post<TagProductView[]>(environment.API_URL + 'print-inproces-tag/postGetProduct', _model).toPromise();
  }

  public async getproductinfo(_code: string) {
    return await this.http.get<PrintInProcessTagView>(environment.API_URL + 'print-inprocess-tag/getProductInfo/' + _code).toPromise(); 
  }
}
