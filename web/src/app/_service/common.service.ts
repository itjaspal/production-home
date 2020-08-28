import { AttachFileView } from './../_model/attach-file-view';
import { AddressDBView } from './../_model/address-dbview';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
//import { ReturnReason } from '../_model/return-reason';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private http: HttpClient) { }

  public postInquiryAddress(_type: string, _txt: string): Observable<AddressDBView> {
    return this.http.post<AddressDBView[]>(environment.API_URL + 'common/postInquiryAddress', {
      type: _type,
      txt: _txt
    })
      .pipe(
        map((res: any) => {
          return res;
        })
      );
  }

  public async postUploadAttachFile(_model: AttachFileView) {

    var fd = new FormData();
    fd.append('file', _model.file);
    fd.append('userId', _model.createUser);
    fd.append('docCode', _model.docCode);
    fd.append('refId', _model.refTransactionId.toString());
    fd.append('typeId', _model.attachFileTypeId.toString());

    return await this.http.post<AttachFileView[]>(environment.API_URL + 'attach/postUploadAttachFile', fd).toPromise();
  }

  public postInquiryAttachFile(_model: AttachFileView) {
    return this.http.post<AttachFileView[]>(environment.API_URL + 'attach/postInquiryAttachFile', _model).toPromise();
  }

  public postDeleteAttachFile(_model: AttachFileView) {
    return this.http.post<AttachFileView[]>(environment.API_URL + 'attach/postDeleteAttachFile', _model).toPromise();
  }

  // public postCreateReturnReason(_model: ReturnReason) {
  //   return this.http.post(environment.API_URL + 'common/postCreateReturnReason', _model).toPromise();
  // }

  // public postUpdateReturnReason(_model: ReturnReason) {
  //   return this.http.post(environment.API_URL + 'common/postUpdateReturnReason', _model).toPromise();
  // }

  // public async getAllReturnReason() {
  //   return await this.http.get<ReturnReason[]>(environment.API_URL + 'common/getAllReturnReason').toPromise();
  // }

}
