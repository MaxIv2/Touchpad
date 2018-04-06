package com.example.maxim2.client;

import android.util.Log;
import android.view.MotionEvent;
import android.view.View;

import java.security.PrivilegedAction;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.RecursiveTask;

/**
 * Created by Maxim2 on 4/5/2018.
 */

public class TouchSurfaceListener implements View.OnTouchListener {
    public TouchSurfaceListener(Client c) {
       this.c = c;
       this.tapTimer = new Timer();
    }

    private Timer tapTimer;
    private boolean tap;


    private Client c;
    private final byte[] tapBuffer = {1,0,1,1};
    private final byte[] bufferMove = {0,0,0};
    private final byte[] bufferScroll = {3,0};
    private final byte[] bufferPinch = {4,0};
    private static final int DX_INDEX = 1;
    private static final int DY_INDEX = 2;
    private static final int EXTRA_INDEX = 1;
    @Override
    public boolean onTouch(View view, MotionEvent motionEvent) {
        if(tap  && motionEvent.getAction() == MotionEvent.ACTION_UP) {
            this.c.addToQueue(tapBuffer);
            Log.d("", "tap that shit");
        } else if(motionEvent.getAction() == MotionEvent.ACTION_DOWN) {
            this.tap = true;
            Log.d("", "timers: roll out");
            this.tapTimer.schedule(new TimerTask() {
                @Override
                public void run() {
                    TouchSurfaceListener.this.tap = false;
                }
            }, 150);
        }
        else if (motionEvent.getPointerCount() == 1)
            this.onOneFinger(motionEvent);
        else if(motionEvent.getPointerCount() == 2)
            this.onTwoFingers(motionEvent);
        return false;
    }

    private void onOneFinger(MotionEvent motionEvent) {
        if (motionEvent.getAction() == MotionEvent.ACTION_MOVE) {
            if(motionEvent.getHistorySize() < 1)
                return;
            this.tap = false;
            int dx = (int) (motionEvent.getX() - motionEvent.getHistoricalX(0));
            int dy = (int) (motionEvent.getY() - motionEvent.getHistoricalY(0));
            bufferMove[DX_INDEX] = (byte) dx;
            bufferMove[DY_INDEX] = (byte) dy;
            c.addToQueue(bufferMove);
        }
    }
    private void onTwoFingers(MotionEvent motionEvent) {
        int action = motionEvent.getAction();
        if(action == MotionEvent.ACTION_MOVE) {
            int pinch = isPinch(motionEvent);
            if(pinch != 0) {
                bufferPinch[EXTRA_INDEX] = (byte) pinch;
                c.addToQueue(bufferPinch);
            } else {
                if(motionEvent.getHistorySize() < 1)
                    return;
                int dy = (int) (motionEvent.getY() - motionEvent.getHistoricalY(0));
                bufferScroll[EXTRA_INDEX] = (byte)  (dy * 5);
                c.addToQueue(bufferScroll);
            }
        }
    }

    private int isPinch(MotionEvent motionEvent) {
        if(motionEvent.getPointerCount() != 2)
            return 0;
        if(motionEvent.getHistorySize() < 1)
            return 0;
        //historical
        MotionEvent.PointerCoords h0 = new MotionEvent.PointerCoords();
        MotionEvent.PointerCoords h1 = new MotionEvent.PointerCoords();
        motionEvent.getHistoricalPointerCoords(0, 0, h0);
        motionEvent.getHistoricalPointerCoords(1, 0, h1);

        //actual
        MotionEvent.PointerCoords a0 = new MotionEvent.PointerCoords();
        MotionEvent.PointerCoords a1 = new MotionEvent.PointerCoords();
        motionEvent.getPointerCoords(0, a0);
        motionEvent.getPointerCoords(1, a1);

        MotionEvent.PointerCoords vector0to1= new MotionEvent.PointerCoords();
        vector0to1.x = a1.x - a0.x;
        vector0to1.y = a1.y - a0.y;

        MotionEvent.PointerCoords vector1Move = new MotionEvent.PointerCoords();
        vector1Move.x = h1.x - a1.x;
        vector1Move.y = h1.y - a1.y;

        MotionEvent.PointerCoords vector0Move = new MotionEvent.PointerCoords();
        vector0Move.x = a0.x - h0.x;
        vector0Move.y = a0.y - h0.y;

        double piBy6 = Math.PI / 6;
        double a = getAngle(vector0Move, vector0to1);
        if(a > piBy6 && a < piBy6 * 5)
            return 0;
        a = getAngle(vector1Move, vector0to1);
        if(a > piBy6 && a < piBy6 * 5)
            return 0;
        double res = Math.sqrt(Math.pow(vector0Move.x+ vector1Move.x, 2) + Math.pow(vector0Move.y+ vector1Move.y, 2));
        if (a <  Math.PI / 6)
            res = -res;
        return (int)res / 2;
    }

    private double getAngle(MotionEvent.PointerCoords vector1, MotionEvent.PointerCoords vector2) {
        float mone = vector1.x * vector2.x + vector1.y * vector2.y;
        double machane = Math.sqrt(Math.pow(vector1.x,2) + Math.pow(vector1.y,2)) * Math.sqrt(Math.pow(vector2.x,2) + Math.pow(vector2.y,2));
        return Math.acos(mone/machane);
    }

}
