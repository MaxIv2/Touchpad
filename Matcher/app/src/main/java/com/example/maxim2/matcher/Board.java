package com.example.maxim2.matcher;

import java.util.List;
import java.util.Random;
import java.util.Vector;

/**
 * Created by Maxim2 on 4/27/2018.
 */

public class Board {
    private CellContent[][] contents;

    public Board(int width, int height) {
        this.contents = new CellContent[height][width];
        Random r = new Random();
        for (int row = 0; row < height; row++) {
            for (int column = 0; column < height; column ++) {
                this.contents[row][column] = new CellContent(CellContent.ContentType.getRandom());
            }
        }
    }

    private MatchDescription CheckCell(int row, int column) {
        MatchDescription m = new MatchDescription();
        m.add(row,column);
        CellContent.ContentType type = contents[row][column].getType();
        for (int i = row + 1; i < contents.length) {
            m[]
        }
        
        return m;
    }

    private class MatchDescription {
        private List<Tuple<Integer,Integer>> matchCoords;
        private int maxX;
        private int maxY;
        private int minX;
        private int minY;

        MatchDescription() {
            this.maxX = -1;
            this.maxY = -1;
            this.minX = -1;
            this.minY = -1;
            matchCoords = new Vector<Tuple<Integer,Integer>>();
        }

        void add(int x, int y) {
            if (maxX < x)
                maxX = x;
            if (maxY < y)
                maxY = y;
            if(minX == -1 || minX > x)
                minX = x;
            if(minX == -1 || minY > y)
                minY = y;
            matchCoords.add(new Tuple<Integer, Integer>(x, y));
        }
    }
}
