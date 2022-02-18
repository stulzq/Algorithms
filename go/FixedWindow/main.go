package main

import (
	"fmt"
	"go.uber.org/atomic"
	"sync"
	"time"
)

func main() {
	wg := sync.WaitGroup{}
	for i := 0; i < 10; i++ {
		wg.Add(1)
		go func() {
			for j := 0; j < 10000; j++ {
				fixedWindow()
			}
			wg.Done()
		}()
	}
	wg.Wait()
	fmt.Println(currentNumber.Load())
	time.Sleep(2 * time.Second)
	fmt.Println(fixedWindow())
}

var currentTime atomic.Int64
var currentNumber atomic.Int64

func fixedWindow() int64 {
	now := time.Now().Unix()
	ct := currentTime.Load()
	if now > ct {
		if currentTime.CAS(ct, now) {
			currentNumber.Store(0)
		}
	}

	return currentNumber.Inc()

}
