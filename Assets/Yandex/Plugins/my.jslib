mergeInto(LibraryManager.library, {
	
	SaveExtern: function (data) {
		console.log('Saving..');
		var dataString = UTF8ToString(data);
		var myObj = JSON.parse(dataString);
		player.setData(myObj).then(function () {
			console.log('Saved');
		});
	},
  
  	LoadExtern: function () {
		console.log('Loading...');
		player.getData().then(function (_data) {
			const myJSON = JSON.stringify(_data);
			myGameInstance.SendMessage('PlayerStats', 'SetProgressInfo', myJSON);
			console.log('Loaded.');
		});
	},
  
	ShowAdv: function () {
		ysdk.adv.showFullscreenAdv({
		callbacks: {
			onClose: function(wasShown) {
				console.log('-------------- Adv has been shown --------------');
			},
			onError: function(error) {
				console.log('-------------- Adv error --------------');
			}
		}
		});
	},

	ShowRewAdv: function () {
		ysdk.adv.showRewardedVideo({callbacks:{}})
	},

});