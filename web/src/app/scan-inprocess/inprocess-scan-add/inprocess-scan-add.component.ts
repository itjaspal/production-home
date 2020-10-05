import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';

@Component({
  selector: 'app-inprocess-scan-add',
  templateUrl: './inprocess-scan-add.component.html',
  styleUrls: ['./inprocess-scan-add.component.scss']
})
export class InprocessScanAddComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    //private _jobInprocessSvc: JobinprocessService,
    //private snackBar: MatSnackBar
  ) { }

  public validationForm: FormGroup;
  public user: any;
  // public model: JobInProcessView = new JobInProcessView();
  // searchModel: JobInProcessSearchView = new JobInProcessSearchView();

  public data: any = {};
  // public model_scan: JobInProcessScanFinView = new JobInProcessScanFinView();

  public datas: any = {};
  public count = 0;
  
  @ViewChild('qr') qrElement:ElementRef;
  ngAfterViewInit(){
    this.qrElement.nativeElement.focus();
    //this.qrElement.nativeElement.dismissSoftInput();
  }

  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
  }

  private buildForm() {
    this.validationForm = this._fb.group({
    qr: [null, []]
    });
  }

  async onQrEntered(_qr: string) {

    if (_qr == null || _qr == "") {
      return;
    }

    var datePipe = new DatePipe("en-US");
    
    // this.searchModel.req_date = this._actRoute.snapshot.params.req_date;
    // this.searchModel.wc_code = this.user.def_wc_code;
    // this.searchModel.pcs_barcode = _qr;
    // this.searchModel.req_date  = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy');
    // this.searchModel.mc_code = this.user.mc_code;
    // this.searchModel.user_id = this.user.username;
    // this.searchModel.spring_grp = this._actRoute.snapshot.params.spring_grp;
    // this.searchModel.springtype_code = this._actRoute.snapshot.params.springtype_code;
    // this.searchModel.size_code = this._actRoute.snapshot.params.size_code;
   


    
    // this.datas = await this._jobInprocessSvc.searchscanpcs(this.searchModel);
    
    this.qrElement.nativeElement.focus();
    
    
    this.add(this.datas);
    
  }

  add(datas: any) {

        // let newProd: JobInProcessScanView = new JobInProcessScanView();
        // newProd.pcs_barcode = datas.pcs_barcode;
        // newProd.prod_code = datas.prod_code;
     
        
        
        // this.model_scan.datas.push(newProd);
        
        // this.count = this.model_scan.datas.length;
  
  }

  close() {
  
    this._router.navigateByUrl('/app/scaninproc/inprocsearch/'+this._actRoute.snapshot.params.req_date);

  }

  


}
