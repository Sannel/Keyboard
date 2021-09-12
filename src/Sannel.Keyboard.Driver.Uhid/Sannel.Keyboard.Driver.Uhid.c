/*
 * C library to create Uhid device and send commands through it
 */
#include <errno.h>
#include <fcntl.h>
#include <poll.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <termios.h>
#include <unistd.h>
#include <linux/uhid.h>
// Usb keyboard rdesc
static unsigned char rdesc[] = {
    0x05, 0x01,
    0x09, 0x06,
    0xa1, 0x01,
    0x05, 0x07,
    0x19, 0xe0,
    0x29, 0xe7,
    0x15, 0x00,
    0x25, 0x01,
    0x75, 0x01,
    0x95, 0x08,
    0x81, 0x02,
    0x95, 0x01,
    0x75, 0x08,
    0x81, 0x01,
    0x95, 0x05,
    0x75, 0x01,
    0x05, 0x08,
    0x19, 0x01,
    0x29, 0x05,
    0x91, 0x02,
    0x95, 0x01,
    0x75, 0x03,
    0x91, 0x01,
    0x95, 0x06,
    0x75, 0x08,
    0x15, 0x00,
    0x26, 0xff,
    0x00, 0x05,
    0x07, 0x19,
    0x00, 0x2a,
    0xff, 0x00,
    0x81, 0x00,
    0xc0 
};

/*
 * Returns /dev/uhid for now
 */
const char* GetUhidFile()
{
	return "/dev/uhid";
}


/*
 * Open up the /dev/uhid device and return the file id from open
 */
int OpenUHID()
{
	int fd = open(GetUhidFile(), O_RDWR | O_CLOEXEC);
	return fd;
}

/*
 * Close the open file
 */
void CloseUHID(int fileId)
{
    close(fileId);
}

/*
 * Writes the event to the uhid device
 */
static int uhid_write(int fd, const struct uhid_event *ev)
{
	ssize_t ret;

	ret = write(fd, ev, sizeof(*ev));
	if (ret < 0) {
		return -errno;
	} else if (ret != sizeof(*ev)) {
		return -EFAULT;
	} else {
		return 0;
	}
}

/*
 * Creates the Hid device with the given name
 */
int CreateDevice(int fileId, char* deviceName)
{
	struct uhid_event ev;

	memset(&ev, 0, sizeof(ev));
	ev.type = UHID_CREATE;
	strcpy((char*)ev.u.create.name, deviceName);
	ev.u.create.rd_data = rdesc;
	ev.u.create.rd_size = sizeof(rdesc);
	ev.u.create.bus = BUS_USB;
	ev.u.create.vendor = 0x15d9;
	ev.u.create.product = 0x0a37;
	ev.u.create.version = 0;
	ev.u.create.country = 0;

    fprintf(stderr, "%s\n", ev.u.create.name);

	int ret = uhid_write(fileId, &ev);

    return ret;
}

/*
 * Destores the hid device
 */
void DestroyDevice(int fileId)
{
	struct uhid_event ev;

	memset(&ev, 0, sizeof(ev));
	ev.type = UHID_DESTROY;

	uhid_write(fileId, &ev);
}

/*
 * Sends a key event through the hid device
 */
int SendEventKey(int fileId, unsigned char key)
{
	struct uhid_event ev;

	memset(&ev, 0, sizeof(ev));
	ev.type = UHID_INPUT;
    ev.u.input.size = 8;

    ev.u.input.data[0] = 0x00;
    ev.u.input.data[1] = 0x00;
	ev.u.input.data[2] = key;
    ev.u.input.data[3] = 0x00;
    ev.u.input.data[4] = 0x00;
    ev.u.input.data[5] = 0x00;
    ev.u.input.data[6] = 0x00;
    ev.u.input.data[7] = 0x00;

	return uhid_write(fileId, &ev);
}
