var net = require('net');
var rl = require('readline');
var prompts = rl.createInterface(process.stdin, process.stdout);

var HOST = '127.0.0.1';
var PORT = 8888;

var client = new net.Socket();
client.connect(PORT, HOST, function() {
    console.log('CONNECTED TO: ' + HOST + ':' + PORT);
	
	prompts.question("Please tell me your name: ", function(input){
		client.write(input+"$$");
		chat();
	});	
	
	client.on('data', function(data) {		
		console.log('' + data);		
	});
});

function chat(){
	prompts.question("", function(input){
		client.write(input+"$$");
		chat();
	});
}


