// Our input frames will come from here.
const videoElement = document.getElementsByClassName('myimg')[0];
const canvasElement = document.getElementsByClassName('output_canvas')[0];
const controlsElement = document.getElementsByClassName('control-panel')[0];

console.log(document.getElementsByClassName('myimg')[0]);

// We'll add this to our control panel later, but we'll save it here so we can
// call tick() each time the graph runs.
const fpsControl = new FPS();


function onResults(results) {

  console.log("收到结果");
  console.log(results.poseLandmarks);


}

const pose = new Pose({locateFile: (file) => {
  return `https://cdn.jsdelivr.net/npm/@mediapipe/pose@0.1/${file}`;
}});
pose.onResults(onResults);

/**
 * Instantiate a camera. We'll feed each frame we receive into the solution.
 */


pose.send({image: videoElement});