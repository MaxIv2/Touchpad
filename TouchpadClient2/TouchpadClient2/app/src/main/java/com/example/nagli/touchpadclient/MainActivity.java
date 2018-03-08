package com.example.nagli.touchpadclient;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;

import java.io.IOException;
import java.io.UnsupportedEncodingException;

public class MainActivity extends AppCompatActivity {

    Button btnLeft,btnRight;
    View view;
    LinearLayout linearLayout;
    String adress;
    BluetoothClient bluetoothClient;
    Context context;
    @SuppressLint("ClickableViewAccessibility")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        btnLeft = (Button)findViewById(R.id.btnleft);
        btnLeft.setOnTouchListener(btnLeftTouchListener);
        btnRight = (Button)findViewById(R.id.btnright);
        btnRight.setOnTouchListener(btnRightTouchListener);
        btnLeft = (Button)findViewById(R.id.btnleft);
        btnLeft.setOnTouchListener(viewTouchListener);
        adress = "C4:8E:8F:C3:61:12";
        try {
            bluetoothClient = new BluetoothClient(adress);
            try {
                bluetoothClient.SetTimer();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        } catch (IOException e) {
            Toast.makeText(this, "not connected!", Toast.LENGTH_LONG).show();
            e.printStackTrace();
        }
        context =this;
       /* try {

            Intent intent = new Intent("com.google.zxing.client.android.SCAN");
            intent.putExtra("SCAN_MODE", "QR_CODE_MODE");

            startActivityForResult(intent, 0);

        } catch (Exception e) {

            Uri marketUri = Uri.parse("market://details?id=com.google.zxing.client.android");
            Intent marketIntent = new Intent(Intent.ACTION_VIEW,marketUri);
            startActivity(marketIntent);

        }*/
    }

    View.OnTouchListener viewTouchListener = new View.OnTouchListener()
    {
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            if (motionEvent.getAction() == motionEvent.ACTION_MOVE){
                byte[] buffer = {0,3,0,(byte) motionEvent.getX(), (byte) motionEvent.getY()};
                bluetoothClient.SetMessage(buffer);
                try {
                    Toast.makeText(context ,new String(buffer, "UTF-8"), Toast.LENGTH_LONG).show();
                } catch (UnsupportedEncodingException e) {
                    e.printStackTrace();
                }
            }
            return true;
        }
    };
    View.OnTouchListener btnLeftTouchListener = new View.OnTouchListener()
    {
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            if (motionEvent.getAction() == motionEvent.ACTION_UP){
                byte[] buffer = {0,2,1,1};
                bluetoothClient.SetMessage(buffer);
            }
            if (motionEvent.getAction() == motionEvent.ACTION_DOWN){
                byte[] buffer = {0,2,1,0};
                bluetoothClient.SetMessage(buffer);
            }
            return true;
        }
    };
    View.OnTouchListener btnRightTouchListener = new View.OnTouchListener()
    {
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            if (motionEvent.getAction() == motionEvent.ACTION_UP){
                byte[] buffer = {0,2,2,1};
                bluetoothClient.SetMessage(buffer);
            }
            if (motionEvent.getAction() == motionEvent.ACTION_DOWN){
                byte[] buffer = {0,2,2,0};
                bluetoothClient.SetMessage(buffer);
            }
            return true;
        }
    };
    /*@Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 0) {

            if (resultCode == RESULT_OK) {
                adress = data.getStringExtra("SCAN_RESULT");
                try {
                    bluetoothClient = new BluetoothClient(adress);
                    try {
                        bluetoothClient.SetTimer();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            if(resultCode == RESULT_CANCELED){
            }
        }
    }
    */
}
