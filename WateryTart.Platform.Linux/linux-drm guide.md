sudo apt update
sudo apt upgrade
sudo reboot
sudo apt-get install libgbm1 libgl1-mesa-dri libegl1-mesa libinput10
sudo apt-get mesa-utils # maybe this one
sudo apt-get install kmscube
sudo kmscube



/boot/firmware/config.txt add
dtoverlay=hifiberry-amp4pro

test with
aplay -l