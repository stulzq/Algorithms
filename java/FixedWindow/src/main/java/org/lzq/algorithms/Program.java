package org.lzq.algorithms;

import java.util.concurrent.atomic.AtomicLong;

public class Program {
    public static void main(String[] args) throws InterruptedException {
        currentTime.set(currentTimeSeconds());
        currentNumber.set(0);

        long num = 0;
        for (int i = 0; i < 1000; i++) {
            num = fixedWindow();
        }
        System.out.println(num);
        Thread.sleep(2000);
        System.out.println(fixedWindow());
    }

    static AtomicLong currentTime = new AtomicLong();
    static AtomicLong currentNumber = new AtomicLong();

    public static long fixedWindow() {
        long now = currentTimeSeconds();
        long ct = currentTime.get();
        if (now > ct) {
            if (currentTime.compareAndSet(ct, now)) {
                currentNumber.set(0);
            }
        }

        return currentNumber.incrementAndGet();
    }

    public static long currentTimeSeconds(){
        return System.currentTimeMillis() / 1000;
    }
}
