import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { JobInProcessScanFinView, JobInProcessScanView, JobInProcessSearchView, JobInProcessView } from '../../_model/job-inprocess';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanInprocessService } from '../../_service/scan-inprocess.service';

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
    private _jobInprocessSvc: ScanInprocessService,
    //private snackBar: MatSnackBar
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: JobInProcessView = new JobInProcessView();
  public searchModel: JobInProcessSearchView = new JobInProcessSearchView();

  public data: any = {};
  public model_scan: JobInProcessScanFinView = new JobInProcessScanFinView();

  public datas: any = {};
  public count = 0;

  public result: any = {};
  
  @ViewChild('qr') qrElement:ElementRef;

  ngAfterViewInit(){
    //this.qrElement.nativeElement.focus();
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
    
     //this.searchModel.req_date = this._actRoute.snapshot.params.req_date;
     this.searchModel.wc_code = this._actRoute.snapshot.params.wc_code;
     this.searchModel.bar_code = _qr;
     this.searchModel.req_date  = datePipe.transform(this._actRoute.snapshot.params.req_date, 'dd/MM/yyyy').toString();
    
     this.searchModel.user_id = this.user.username;
     this.searchModel.build_type = this.user.branch.entity_code;
     this.searchModel.pdjit_grp = this._actRoute.snapshot.params.pdjit_grp;


    
    this.datas = await this._jobInprocessSvc.searchscanadd(this.searchModel);
    
    this.qrElement.nativeElement.focus();
    

    this.add(this.datas);
    
  }

  add(datas: any) {

        let newProd: JobInProcessScanView = new JobInProcessScanView();
        newProd.prod_code = datas.prod_code;
        newProd.prod_name = datas.prod_name;
        newProd.qty = datas.qty;
     
        
        this.model_scan.datas.push(newProd);
        
        // Group By Product
        var result = [];
        this.model_scan.datas.forEach(function (a) {
          if ( !this[a.prod_code] && !this[a.prod_name] ) {
              this[a.prod_code] = { prod_code: a.prod_code, prod_name: a.prod_name, qty: 0 };
              result.push(this[a.prod_code]);
          } 
          this[a.prod_code].qty += a.qty;
          
        }, Object.create(null));
       
        this.model_scan.datas = result;
        //console.log(result)
        console.log(this.model_scan.datas);
        
  
  }

  close() { 
    this._router.navigateByUrl('/app/scaninproc/inprocserach/'+this._actRoute.snapshot.params.req_date+'/'+this._actRoute.snapshot.params.wc_code);
  }

  


}
