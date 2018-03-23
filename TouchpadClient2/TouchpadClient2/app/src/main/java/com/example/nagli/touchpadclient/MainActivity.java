package com.example.nagli.touchpadclient;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;

import java.io.IOException;
import java.io.UnsupportedEncodingException;

public class MainActivity extends AppCompatActivity {

    Button btnLeft,btnRight,btnExit;
    View view;
    LinearLayout linearLayout;
    String adress;
    BluetoothClient bluetoothClient;
    Context context;
    Float previousX, previousY;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        btnLeft = (Button)findViewById(R.id.btnleft);
        btnLeft.setOnTouchListener(btnLeftTouchListener);
        btnRight = (Button)findViewById(R.id.btnright);
        btnRight.setOnTouchListener(btnRightTouchListener);
        btnExit = (Button)findViewById(R.id.btnexit);
        btnExit.setOnClickListener(btnExitOnClickListener);
        view = (View) findViewById(R.id.view);
        view.setOnTouchListener(viewTouchListener);
        adress = "08:3E:8E:83:05:2A";
        try {
            bluetoothClient = new BluetoothClient(adress);
            try {
                bluetoothClient.SetMessageTimer();
                bluetoothClient.SetConnectivityCheckerTimer();
                bluetoothClient.SetACKtimer();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        } catch (IOException e) {
            Toast.makeText(this, "not connected!", Toast.LENGTH_LONG).show();
            e.printStackTrace();
        }
        context =this;
    }

    View.OnTouchListener viewTouchListener = new View.OnTouchListener()
    {
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            Log.println(Log.INFO,"move", "Called");
            if (motionEvent.getAction() == MotionEvent.ACTION_DOWN) {
                previousX = motionEvent.getX();
                previousY = motionEvent.getY();
                Log.println(Log.INFO,"move", "down: " + previousX + ", " + previousY);
            }
            if (motionEvent.getAction() == MotionEvent.ACTION_MOVE){
                byte[] buffer = {0,(byte) (motionEvent.getX()-previousX), (byte) (motionEvent.getY()-previousY)};
                bluetoothClient.SetMessage(buffer);
                previousX= motionEvent.getX();
                previousY= motionEvent.getY();
                Log.println(Log.INFO,"move", "move: " + buffer[1] + ", " + buffer[2]);
            }
            if(motionEvent.getAction() == MotionEvent.ACTION_UP) {
                view.performClick();
            }
            return true;
        }
    };
    View.OnTouchListener btnLeftTouchListener = new View.OnTouchListener()
    {
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            Log.println(Log.INFO,"btnLeftTouchListener()", "Called");
            if (motionEvent.getAction() == MotionEvent.ACTION_UP){
                view.performClick();
                Log.println(Log.INFO,"left", "Up");
                byte[] buffer = {1,1};
                bluetoothClient.SetMessage(buffer);
            }
            if (motionEvent.getAction() == MotionEvent.ACTION_DOWN){
                Log.println(Log.INFO,"left", "Down");
                byte[] buffer = {1,0};
                bluetoothClient.SetMessage(buffer);
            }
            return true;
        }
    };
    View.OnTouchListener btnRightTouchListener = new View.OnTouchListener()
    {
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            if (motionEvent.getAction() == MotionEvent.ACTION_UP){
                Log.println(Log.INFO,"right", "Up");
                view.performClick();
                byte[] buffer = {2,1};
                bluetoothClient.SetMessage(buffer);
            }
            if (motionEvent.getAction() == MotionEvent.ACTION_DOWN){
                Log.println(Log.INFO,"right", "Up");
                byte[] buffer = {2,0};
                bluetoothClient.SetMessage(buffer);
            }
            return true;
        }
    };
    View.OnClickListener btnExitOnClickListener = new View.OnClickListener()
    {
        @Override
        public void onClick(View view) {
            if(btnExit == view) {
                byte [] buffer = {3,0};
                bluetoothClient.sendData(buffer);
                bluetoothClient.close();
                switchActivity(StartActivity.class);
            }
        }
    };
    public void switchActivity(Class C)
    {
        Intent intent = new Intent(this, C);
        startActivity(intent);
    }

}
