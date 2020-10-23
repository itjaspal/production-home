import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SetNoSearchView } from '../../_model/scan-send';
import { ScanSendService } from '../../_service/scan-send.service';

@Component({
  selector: 'app-scan-send-setno-search',
  templateUrl: './scan-send-setno-search.component.html',
  styleUrls: ['./scan-send-setno-search.component.scss']
})
export class ScanSendSetnoSearchComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: SetNoSearchView,
    private _fb: FormBuilder,
    private _scanSendSvc: ScanSendService,
    private cdr: ChangeDetectorRef,) { }

    public validationForm: FormGroup;
    user:any = {};
    public model : SetNoSearchView  = new SetNoSearchView();
    //datas: any;
    datas: any = {};
   
  
    async ngOnInit() {
      var datePipe = new DatePipe("en-US");
      this.model.tran_date  = datePipe.transform(this.data.tran_date, 'dd/MM/yyyy').toString();
      //this.model.req_date = this.data.req_date; 
      this.model.entity = this.data.entity; 
      this.model.wc_code = this.data.wc_code;
      console.log(this.model);
  
      this.datas = await this._scanSendSvc.searchsetno(this.model);
      
      console.log(this.datas);
     
          
    }
  
  
    add()
    { 
      let addList: any = this.datas.filter(x => x.selected);
      console.log(addList);
  
      this.dialogRef.close(addList);
    }
  
    close()
    {
      this.dialogRef.close([]);
    }

}
